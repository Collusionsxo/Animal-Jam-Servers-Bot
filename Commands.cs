using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using System;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace AJ_Servers_Bot
{

    public class Basic : BaseCommandModule
    {
        public class SlashCommands : ApplicationCommandModule
        {
            [SlashCommand("Status", "Checks the status of the Animal Jam servers.")]
            [Description("Checks the status of the Animal Jam servers.")]
            public async Task CheckServers(InteractionContext ctx)
            {
                await ctx.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Checking Animal Jam server status...")); //Sends a response message in the channel the command was used in.
                using (var req = new Leaf.xNet.HttpRequest()
                {
                    KeepAlive = true,
                    IgnoreProtocolErrors = true,
                })
                {
                    req.SslCertificateValidatorCallback = (RemoteCertificateValidationCallback)Delegate.Combine(req.SslCertificateValidatorCallback, new RemoteCertificateValidationCallback((object s_, X509Certificate r_, X509Chain N_, SslPolicyErrors o_) => ((X509Certificate2)r_).Verify()));
                    req.AddHeader("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) AJClassic/1.5.7 Chrome/87.0.4280.141 Electron/11.5.0 Safari/537.36");
                    Leaf.xNet.HttpResponse reglogin = req.Post($"https://authenticator.animaljam.com/authenticate", $"{{\"username\":\"test\",\"password\":\"test\",\"domain\":\"flash\",\"df\":\"BOT\"}}", "application/json"); //Sends a post request to the Animal Jam API.
                    switch (reglogin.ToString().Contains("403 Forbidden") || reglogin.ToString().Contains("Service Unavailable")) //RETRY
                    {
                        case true:
                            DiscordEmbedBuilder retryembed = new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Yellow,
                                Description = $"Error in the request! Please retry.",
                                Title = $"⚠️ Error!",
                                Footer = new DiscordEmbedBuilder.EmbedFooter
                                {
                                    IconUrl = "https://cdn.discordapp.com/avatars/1038814695911591956/a_5d11246c76af06be310ad83aca921644?size=1024",
                                    Text = $"Made by @collusions",
                                }
                            };
                            await ctx.Channel.SendMessageAsync(retryembed); //Sends the embed indicating the status.
                            break;
                    }
                    switch (reglogin.ToString().Contains($"error_code\":101")) //Online
                    {
                        case true:
                            DiscordEmbedBuilder online = new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Green,
                                Description = $"The Animal Jam servers are **online and operational**.",
                                Title = $"✅ Online",
                                Footer = new DiscordEmbedBuilder.EmbedFooter
                                {
                                    IconUrl = "https://cdn.discordapp.com/avatars/1038814695911591956/a_5d11246c76af06be310ad83aca921644?size=1024",
                                    Text = $"Made by @collusions",
                                    
                                }
                            };
                            await ctx.Channel.SendMessageAsync(online);
                            break;
                    }
                    switch (reglogin.ToString().Equals("{}")) //Offline
                    {
                        case true:
                            DiscordEmbedBuilder offline = new DiscordEmbedBuilder
                            {
                                Color = DiscordColor.Red,
                                Description = $"The Animal Jam servers are currently **offline and unusable**.",
                                Title = $"❌ Offline",
                                Footer = new DiscordEmbedBuilder.EmbedFooter
                                {
                                    IconUrl = "https://cdn.discordapp.com/avatars/1038814695911591956/a_5d11246c76af06be310ad83aca921644?size=1024",
                                    Text = $"Made by @collusions",
                                }
                            };
                            await ctx.Channel.SendMessageAsync(offline); //Sends the embed indicating the status.
                            break;
                    }
                }

            }
        }
    }
}