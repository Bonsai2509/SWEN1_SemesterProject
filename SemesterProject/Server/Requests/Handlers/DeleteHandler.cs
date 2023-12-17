using Newtonsoft.Json.Linq;
using Npgsql;
using SemesterProject.Cards;
using SemesterProject.Server.Models;
using SemesterProject.Server.Responses;
using SemesterProject.Server.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SemesterProject.Server.Requests.Handlers
{
    internal class DeleteHandler : Handler
    {
        public Response HandleDelete(Request request)
        {
            if (request.Target[0] == "tradings")
            {
                return DeleteTrade(request);
            }
            else
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().BadRequest();
            }
        }

        private Response DeleteTrade(Request request)
        {
            var security = new UserAuthorizer();
            if (!security.AuthorizeUserByToken(request))
            {
                Database.DisposeDbConnection();
                return new ResponseBuilder().Unauthorized();
            }
            else
            {
                if(request.Target.Length != 2)
                {
                    Database.DisposeDbConnection();
                    return new ResponseBuilder().BadRequest();
                }
                string idString = request.Target[1];
                Guid tradeId = Guid.Parse(idString);
                TradeDetails trade = null;
                try
                {
                    using var command = new NpgsqlCommand(@"SELECT ""tradeid"", ""cardid"", ""tradedtype"", ""tradeddmg"" FROM ""trades"" WHERE ""tradeid""=@p1;", Connection);
                    command.Parameters.AddWithValue("p1", tradeId);
                    command.Prepare();
                    using var reader = command.ExecuteReader();
                    if(!reader.Read())
                    {
                        Database.DisposeDbConnection();
                        return new ResponseBuilder().NotFound();
                    }
                    else
                    {

                        trade = new TradeDetails(
                            reader.GetGuid(0),
                            reader.GetGuid(1),
                            reader.GetString(2),
                            reader.GetInt32(3)
                            );
                        reader.Close();
                        command.Dispose();
                        var utils = new Utility();
                        string username = utils.ExtractUsernameFromToken(utils.ExtractTokenFromString(request.Headers["Authorization"]));
                        using var command2 = new NpgsqlCommand(@"SELECT * FROM ""stack"" WHERE ""username""=@p1 AND ""cardid""=@p2;", Connection);
                        command2.Parameters.AddWithValue("p1", username);
                        command2.Parameters.AddWithValue("p2", trade.CardId);
                        command2.Prepare();
                        using var reader2 = command2.ExecuteReader();
                        if(!reader2.Read())
                        {
                            Database.DisposeDbConnection();
                            return new ResponseBuilder().Forbidden(); 
                        }
                        else
                        {
                            reader2.Close();
                            command2.Dispose();
                            using var command3 = new NpgsqlCommand(@"DELETE FROM ""trades"" WHERE ""tradeid""=@p1;", Connection);
                            command3.Parameters.AddWithValue("p1", tradeId);
                            command3.Prepare();
                            var affectedRows = command3.ExecuteNonQuery();
                            if (affectedRows == 0)
                            {
                                Database.DisposeDbConnection();
                                return new ResponseBuilder().Conflict();
                            }
                            else
                            {
                                Database.DisposeDbConnection();
                                return new ResponseBuilder().OK();
                            }
                        }
                    }

                }
                catch (PostgresException ex)
                {
                    return new ResponseBuilder().InternalServerError();
                }
                catch (Exception)
                {
                    return new ResponseBuilder().InternalServerError();
                }
                finally { Database.DisposeDbConnection(); }
            }
        }
    }
}
