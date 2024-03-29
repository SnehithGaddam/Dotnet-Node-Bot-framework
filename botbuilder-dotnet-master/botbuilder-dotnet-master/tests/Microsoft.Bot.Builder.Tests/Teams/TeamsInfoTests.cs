﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Tests;
using Microsoft.Bot.Connector;
using Microsoft.Bot.Connector.Authentication;
using Microsoft.Bot.Schema;
using Microsoft.Bot.Schema.Teams;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json.Linq;

namespace Microsoft.Bot.Builder.Teams.Tests
{
    [TestClass]
    public class TeamsInfoTests
    {
        [TestMethod]
        public async Task TestGetTeamDetailsAsync()
        {
            var baseUri = new Uri("https://test.coffee");
            var customHttpClient = new HttpClient(new RosterHttpMessageHandler());

            // Set a special base address so then we can make sure the connector client is honoring this http client
            customHttpClient.BaseAddress = baseUri;
            var connectorClient = new ConnectorClient(new Uri("http://localhost/"), new MicrosoftAppCredentials(string.Empty, string.Empty), customHttpClient);

            var activity = new Activity
            {
                Type = "message",
                Text = "Test-GetTeamDetailsAsync",
                ChannelId = Channels.Msteams,
                ChannelData = new TeamsChannelData
                {
                    Team = new TeamInfo
                    {
                        Id = "team-id",
                    },
                },
            };

            var turnContext = new TurnContext(new SimpleAdapter(), activity);
            turnContext.TurnState.Add<IConnectorClient>(connectorClient);

            var handler = new TestTeamsActivityHandler();
            await handler.OnTurnAsync(turnContext);
        }

        [TestMethod]
        public async Task TestTeamGetMembersAsync()
        {
            var baseUri = new Uri("https://test.coffee");
            var customHttpClient = new HttpClient(new RosterHttpMessageHandler());

            // Set a special base address so then we can make sure the connector client is honoring this http client
            customHttpClient.BaseAddress = baseUri;
            var connectorClient = new ConnectorClient(new Uri("http://localhost/"), new MicrosoftAppCredentials(string.Empty, string.Empty), customHttpClient);

            var activity = new Activity
            {
                Type = "message",
                Text = "Test-Team-GetMembersAsync",
                ChannelId = Channels.Msteams,
                ChannelData = new TeamsChannelData
                {
                    Team = new TeamInfo
                    {
                        Id = "team-id",
                    },
                },
            };

            var turnContext = new TurnContext(new SimpleAdapter(), activity);
            turnContext.TurnState.Add<IConnectorClient>(connectorClient);

            var handler = new TestTeamsActivityHandler();
            await handler.OnTurnAsync(turnContext);
        }

        [TestMethod]
        public async Task TestGroupChatGetMembersAsync()
        {
            var baseUri = new Uri("https://test.coffee");
            var customHttpClient = new HttpClient(new RosterHttpMessageHandler());

            // Set a special base address so then we can make sure the connector client is honoring this http client
            customHttpClient.BaseAddress = baseUri;
            var connectorClient = new ConnectorClient(new Uri("http://localhost/"), new MicrosoftAppCredentials(string.Empty, string.Empty), customHttpClient);

            var activity = new Activity
            {
                Type = "message",
                Text = "Test-GroupChat-GetMembersAsync",
                ChannelId = Channels.Msteams,
                Conversation = new ConversationAccount { Id = "conversation-id" },
            };

            var turnContext = new TurnContext(new SimpleAdapter(), activity);
            turnContext.TurnState.Add<IConnectorClient>(connectorClient);

            var handler = new TestTeamsActivityHandler();
            await handler.OnTurnAsync(turnContext);
        }

        [TestMethod]
        public async Task TestGetChannelsAsync()
        {
            var baseUri = new Uri("https://test.coffee");
            var customHttpClient = new HttpClient(new RosterHttpMessageHandler());

            // Set a special base address so then we can make sure the connector client is honoring this http client
            customHttpClient.BaseAddress = baseUri;
            var connectorClient = new ConnectorClient(new Uri("http://localhost/"), new MicrosoftAppCredentials(string.Empty, string.Empty), customHttpClient);

            var activity = new Activity
            {
                Type = "message",
                Text = "Test-GetTeamDetailsAsync",
                ChannelId = Channels.Msteams,
                ChannelData = new TeamsChannelData
                {
                    Team = new TeamInfo
                    {
                        Id = "team-id",
                    },
                },
            };

            var turnContext = new TurnContext(new SimpleAdapter(), activity);
            turnContext.TurnState.Add<IConnectorClient>(connectorClient);

            var handler = new TestTeamsActivityHandler();
            await handler.OnTurnAsync(turnContext);
        }

        private class TestTeamsActivityHandler : TeamsActivityHandler
        {
            public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
            {
                await base.OnTurnAsync(turnContext, cancellationToken);

                switch (turnContext.Activity.Text)
                {
                    case "Test-GetTeamDetailsAsync":
                        await CallGetTeamDetailsAsync(turnContext);
                        break;
                    case "Test-Team-GetMembersAsync":
                        await CallTeamGetMembersAsync(turnContext);
                        break;
                    case "Test-GroupChat-GetMembersAsync":
                        await CallGroupChatGetMembersAsync(turnContext);
                        break;
                    case "Test-GetChannelsAsync":
                        await CallGetChannelsAsync(turnContext);
                        break;
                    default:
                        Assert.IsTrue(false);
                        break;
                }
            }

            private async Task CallGetTeamDetailsAsync(ITurnContext turnContext)
            {
                var teamDetails = await TeamsInfo.GetTeamDetailsAsync(turnContext);

                Assert.AreEqual("team-id", teamDetails.Id);
                Assert.AreEqual("team-name", teamDetails.Name);
                Assert.AreEqual("team-aadgroupid", teamDetails.AadGroupId);
            }

            private async Task CallTeamGetMembersAsync(ITurnContext turnContext)
            {
                var members = (await TeamsInfo.GetMembersAsync(turnContext)).ToArray();

                Assert.AreEqual("id-1", members[0].Id);
                Assert.AreEqual("name-1", members[0].Name);
                Assert.AreEqual("givenName-1", members[0].GivenName);
                Assert.AreEqual("surname-1", members[0].Surname);
                Assert.AreEqual("userPrincipalName-1", members[0].UserPrincipalName);

                Assert.AreEqual("id-2", members[1].Id);
                Assert.AreEqual("name-2", members[1].Name);
                Assert.AreEqual("givenName-2", members[1].GivenName);
                Assert.AreEqual("surname-2", members[1].Surname);
                Assert.AreEqual("userPrincipalName-2", members[1].UserPrincipalName);
            }

            private async Task CallGroupChatGetMembersAsync(ITurnContext turnContext)
            {
                var members = (await TeamsInfo.GetMembersAsync(turnContext)).ToArray();

                Assert.AreEqual("id-3", members[0].Id);
                Assert.AreEqual("name-3", members[0].Name);
                Assert.AreEqual("givenName-3", members[0].GivenName);
                Assert.AreEqual("surname-3", members[0].Surname);
                Assert.AreEqual("userPrincipalName-3", members[0].UserPrincipalName);

                Assert.AreEqual("id-4", members[1].Id);
                Assert.AreEqual("name-4", members[1].Name);
                Assert.AreEqual("givenName-4", members[1].GivenName);
                Assert.AreEqual("surname-4", members[1].Surname);
                Assert.AreEqual("userPrincipalName-4", members[1].UserPrincipalName);
            }

            private async Task CallGetChannelsAsync(ITurnContext turnContext)
            {
                var channels = (await TeamsInfo.GetChannelsAsync(turnContext)).ToArray();

                Assert.AreEqual("channel-id-1", channels[0].Id);

                Assert.AreEqual("channel-id-2", channels[1].Id);
                Assert.AreEqual("channel-name-2", channels[1].Name);

                Assert.AreEqual("channel-id-3", channels[2].Id);
                Assert.AreEqual("channel-name-3", channels[2].Name);
            }
        }

        private class RosterHttpMessageHandler : HttpMessageHandler
        {
            protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);

                // GetTeamDetails
                if (request.RequestUri.PathAndQuery.EndsWith("team-id"))
                {
                    var content = new JObject
                    {
                        new JProperty("id", "team-id"),
                        new JProperty("name", "team-name"),
                        new JProperty("aadGroupId", "team-aadgroupid"),
                    };
                    response.Content = new StringContent(content.ToString());
                }

                // GetChannels
                else if (request.RequestUri.PathAndQuery.EndsWith("team-id/conversations"))
                {
                    var content = new JArray
                    {
                        new JObject { new JProperty("id", "channel-id-1") },
                        new JObject { new JProperty("id", "channel-id-2"), new JProperty("name", "channel-name-2") },
                        new JObject { new JProperty("id", "channel-id-3"), new JProperty("name", "channel-name-3") },
                    };
                    response.Content = new StringContent(content.ToString());
                }

                // GetMembers (Team)
                else if (request.RequestUri.PathAndQuery.EndsWith("team-id/members"))
                {
                    var content = new JArray
                    {
                        new JObject
                        {
                            new JProperty("id", "id-1"),
                            new JProperty("objectId", "objectId-1"),
                            new JProperty("name", "name-1"),
                            new JProperty("givenName", "givenName-1"),
                            new JProperty("surname", "surname-1"),
                            new JProperty("email", "email-1"),
                            new JProperty("userPrincipalName", "userPrincipalName-1"),
                            new JProperty("tenantId", "tenantId-1"),
                        },
                        new JObject
                        {
                            new JProperty("id", "id-2"),
                            new JProperty("objectId", "objectId-2"),
                            new JProperty("name", "name-2"),
                            new JProperty("givenName", "givenName-2"),
                            new JProperty("surname", "surname-2"),
                            new JProperty("email", "email-2"),
                            new JProperty("userPrincipalName", "userPrincipalName-2"),
                            new JProperty("tenantId", "tenantId-2"),
                        },
                    };
                    response.Content = new StringContent(content.ToString());
                }

                // GetMembers (Group Chat)
                else if (request.RequestUri.PathAndQuery.EndsWith("conversation-id/members"))
                {
                    var content = new JArray
                    {
                        new JObject
                        {
                            new JProperty("id", "id-3"),
                            new JProperty("objectId", "objectId-3"),
                            new JProperty("name", "name-3"),
                            new JProperty("givenName", "givenName-3"),
                            new JProperty("surname", "surname-3"),
                            new JProperty("email", "email-3"),
                            new JProperty("userPrincipalName", "userPrincipalName-3"),
                            new JProperty("tenantId", "tenantId-3"),
                        },
                        new JObject
                        {
                            new JProperty("id", "id-4"),
                            new JProperty("objectId", "objectId-4"),
                            new JProperty("name", "name-4"),
                            new JProperty("givenName", "givenName-4"),
                            new JProperty("surname", "surname-4"),
                            new JProperty("email", "email-4"),
                            new JProperty("userPrincipalName", "userPrincipalName-4"),
                            new JProperty("tenantId", "tenantId-4"),
                        },
                    };
                    response.Content = new StringContent(content.ToString());
                }

                return Task.FromResult(response);
            }
        }
    }
}
