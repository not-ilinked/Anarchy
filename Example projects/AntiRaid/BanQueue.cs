using Discord;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AntiRaid
{
    public class BanQueue
    {
        private readonly List<ulong> _enlistedUsers;
        private readonly List<GuildMember> _raiders;

        public BanQueue()
        {
            _enlistedUsers = new List<ulong>();
            _raiders = new List<GuildMember>();
        }

        public void Enqueue(GuildMember member)
        {
            if (!_enlistedUsers.Contains(member.User.Id))
            {
                _enlistedUsers.Add(member.User.Id);
                _raiders.Add(member);
            }
        }

        public void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (_raiders.Count > 0)
                    {
                        GuildMember raider = _raiders[0];

                        try
                        {
                            Console.WriteLine("Banning " + raider.User.ToString());

                            raider.Ban("Attempted raid", 1);
                        }
                        catch (DiscordHttpException ex)
                        {
                            Console.WriteLine("Error occured while banning user: " + ex.Message);
                        }

                        _raiders.RemoveAt(0);
                    }
                    else Thread.Sleep(10);
                }
            });
        }
    }
}
