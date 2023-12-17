using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SemesterProject.Server;
using SemesterProject.Server.Models;
using SemesterProject.Server.Requests;
using SemesterProject.Server.Security;
using SemesterProject.Server.Responses;

namespace SemesterProject.Test
{
    [TestFixture]
    internal class ServerTest
    {
        [TestCase("get", Method.Get)]
        [TestCase("put", Method.Put)]
        [TestCase("post", Method.Post)]
        [TestCase("delete", Method.Delete)]
        public void GetMethodUtilityTest(string method, Method enumMethod)
        {
            var utils = new Utility();
            Assert.That(utils.GetMethod(method), Is.EqualTo(enumMethod));
            Assert.Pass("Utility getMethod works");
        }

        [Test]
        public void UtilityExtractTokenFromStringTest()
        {
            string authorizationHeader = "Bearer kienboec-mtcg";
            var utils = new Utility();
            Assert.That(utils.ExtractTokenFromString(authorizationHeader), Is.EqualTo("kienboec-mtcg"));
            Assert.Pass("Utility extract token from string works");
        }

        [Test]
        public void UtilityExtractUsernameFromTokenTest()
        {
            string token = "kienboec-mtcg";
            var utils = new Utility();
            Assert.That(utils.ExtractUsernameFromToken(token), Is.EqualTo("kienboec"));
            Assert.Pass("Utility extract username from token works");
        }

        [TestCase(Status.Ok)]
        [TestCase(Status.BadRequest)]
        [TestCase(Status.MethodNotAllowed)]
        [TestCase(Status.NotFound)]
        [TestCase(Status.Unauthorized)]
        [TestCase(Status.InternalServerError)]
        [TestCase(Status.Forbidden)]
        [TestCase(Status.Conflict)]
        [TestCase(Status.Created)]
        [TestCase(Status.NoContent)]
        public void ResponseBuilderCodeTest(Status status)
        {
            var respBuilder = new ResponseBuilder();
            Response response = null;
            switch(status)
            {
                case Status.Ok:
                    response = respBuilder.OK();
                    break;
                case Status.BadRequest:
                    response = respBuilder.BadRequest();
                    break;
                case Status.MethodNotAllowed:
                    response = respBuilder.MethodNotAllowed();
                    break;
                case Status.NotFound:
                    response = respBuilder.NotFound();
                    break;
                case Status.Unauthorized:
                    response = respBuilder.Unauthorized();
                    break;
                case Status.InternalServerError:
                    response = respBuilder.InternalServerError();
                    break;
                case Status.Forbidden:
                    response = respBuilder.Forbidden();
                    break;
                case Status.Conflict:
                    response = respBuilder.Conflict();
                    break;
                case Status.Created:
                    response = respBuilder.Created();
                    break;
                case Status.NoContent:
                    response = respBuilder.NoContent();
                    break;
                default:break;
            } 
            Assert.That(response.Status, Is.EqualTo((int)status));
            Assert.That(response.StatusString, Is.EqualTo(status.ToString()));
            Assert.That(response.hasBody, Is.False);
            Assert.Pass("responseBuilder for responses without payload works");
        }

        [Test]
        public void JsonResponseUserDataTest()
        {
            var userData = new UserData("kienboec", "codin...", ":-)");
            var respBuilder = new ResponseBuilder();
            Response response = respBuilder.JsonResponse(userData);
            Assert.That(response.hasBody, Is.True);
            Assert.That(response.StatusString, Is.EqualTo("Ok"));
            Assert.That(response.Status, Is.EqualTo(200));
            Assert.That(response.Payload, Is.EqualTo(JsonConvert.SerializeObject(userData)));
            Assert.That(response.ContentType, Is.EqualTo("application/json"));
        }

        [Test]
        public void JsonResponseCardListTest()
        {
            List<CardData> cardList = new List<CardData>();
            cardList.Add(new CardData(Guid.Parse("c696d0c7-1482-4eae-8262-3f8c52811e39"), "card1", "card1description", 20.0f));
            cardList.Add(new CardData(Guid.Parse("3705c6d8-9ce9-41bf-b577-1cf70a8e1a1d"), "card2", "card2description", 25.0f));
            var respBuilder = new ResponseBuilder();
            Response response = respBuilder.JsonResponseCardList(cardList);
            Assert.That(response.hasBody, Is.True);
            Assert.That(response.StatusString, Is.EqualTo("Ok"));
            Assert.That(response.Status, Is.EqualTo(200));
            Assert.That(response.Payload, Is.EqualTo(JsonConvert.SerializeObject(cardList)));
            Assert.That(response.ContentType, Is.EqualTo("application/json"));
        }

        [Test]
        public void JsonResponseUserScoresListTest()
        {
            List<UserScores> userScoreList = new List<UserScores>();
            userScoreList.Add(new UserScores("user1", 1000));
            userScoreList.Add(new UserScores("user2", 1200));
            var respBuilder = new ResponseBuilder();
            Response response = respBuilder.JsonResponseUserScoreList(userScoreList);
            Assert.That(response.hasBody, Is.True);
            Assert.That(response.StatusString, Is.EqualTo("Ok"));
            Assert.That(response.Status, Is.EqualTo(200));
            Assert.That(response.Payload, Is.EqualTo(JsonConvert.SerializeObject(userScoreList)));
            Assert.That(response.ContentType, Is.EqualTo("application/json"));
        }

        [Test]
        public void JsonResponseUserTradeListTest()
        {
            List<TradeDetails> tradeList = new List<TradeDetails>();
            tradeList.Add(new TradeDetails(Guid.Parse("34627f87-af32-4550-953c-5d339b97cfb5"), Guid.Parse("f59aa8dc-9f4d-43ea-80b7-c4859660664a"), "monster", 20.0f));
            tradeList.Add(new TradeDetails(Guid.Parse("d1180424-e73a-4ee4-a621-ecd80b1e6b28"), Guid.Parse("3f234100-2535-4e48-9fa4-9ce69d24c4f3"), "spell", 30.0f));
            var respBuilder = new ResponseBuilder();
            Response response = respBuilder.JsonResponseUserTradeList(tradeList);
            Assert.That(response.hasBody, Is.True);
            Assert.That(response.StatusString, Is.EqualTo("Ok"));
            Assert.That(response.Status, Is.EqualTo(200));
            Assert.That(response.Payload, Is.EqualTo(JsonConvert.SerializeObject(tradeList)));
            Assert.That(response.ContentType, Is.EqualTo("application/json"));
        }

        [Test]
        public void PlainTextResponseTest()
        {
            List<CardData> cardList = new List<CardData>();
            cardList.Add(new CardData(Guid.Parse("c696d0c7-1482-4eae-8262-3f8c52811e39"), "card1", "card1description", 20.0f));
            cardList.Add(new CardData(Guid.Parse("3705c6d8-9ce9-41bf-b577-1cf70a8e1a1d"), "card2", "card2description", 25.0f));
            int cardCounter = 1;
            string plainText = null;
            foreach (CardData card in cardList)
            {
                plainText = $"{plainText} Card {cardCounter}: UUID: {card.CardId}, Name: {card.CardName}, Damage: {card.CardDamage}, Description: {card.CardDescription}\n";
                cardCounter++;
            }
            var respBuilder = new ResponseBuilder();
            Response response = respBuilder.PlainTextResponse(plainText);
            Assert.That(response.hasBody, Is.True);
            Assert.That(response.StatusString, Is.EqualTo("Ok"));
            Assert.That(response.Status, Is.EqualTo(200));
            Assert.That(response.Payload, Is.EqualTo(plainText));
            Assert.That(response.ContentType, Is.EqualTo("text/plain"));
        }
    }
}
