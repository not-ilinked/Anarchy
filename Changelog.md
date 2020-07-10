(Please keep in mind that these are only the major/important changes. There have been way more than written here)<br>

# idk what to name this version yet
### Additions
- Added async methods (with the same name as their sync methods but the suffix 'Async').

### Improvements
- Improved the way Anarchy deals with purchasing subscriptions.
- Removed various useless/bloaty methods (GetGuildRole, Mute/DeafenGuildMember and so on).
- In progress: Removed redundant info from method names such as: CreateChannelWebhook -> CreateWebhook.



# 0.7.2.0
### Additions
- Added a new property 'Member' to SocketGuild, which returns the client's member for that guild.
- Added a cooldown to prevent [Rate Limits](https://discord.com/developers/docs/topics/gateway#rate-limiting) from happening to DiscordSocketClients.
- Added 'ConnectedAccounts' and 'Presences' to LoginEventArgs.

### Improvements
- Improved Anarchy's Connections functionality, adding 'ClientConnectedAccount' for connections the current user owns.
- Improved 'DiscordPresence', putting all the guild-only stuff in 'DiscordGuildPresence'. The method 'ToGuildPresence' has been added to 'DiscordPresence'.
- Improve voice state caching, changing 'GetPrivateChannelVoiceStates' to 'GetChannelVoiceStates' and adding 'GetVoiceState' for individual users.
- 'GuildMember.GetPermissions' will now return all permissions if the current user owns the guild.
- Updated the 'GuildMember' and 'PartialGuildMember' classes. This means that 'OnUserJoinedGuild', 'OnGuildMemberUpdated' and 'OnUserLeftGuild' have been updated as well.
- client.GetChannelMessages now supports Limit filters bigger than 100.
- Updated 'QueryGuilds' to also return the total amount of guilds.
- Renamed 'Group' to 'DiscordGroup' and 'Ban' to 'DiscordBan'.
- Now if the last parameter of a command is a string, more than 1 word will be accepted.



# 0.7.1.2
### Improvements
- Updated some outdated methods.
- Removed socket client debug log hell (i'm sorry for that btw :/).



# 0.7.1.1
### Additions
- Added [Gateway Intents](https://discord.com/developers/docs/topics/gateway#gateway-intents).

### Improvements
- Split ModifyChannel up into ModifyGroup and ModifyGuildChannel to prevent modifying a DM channel.
- Fixed a bug with DiscordImages.



# 0.7.1.0
### Additions
- Added support for modifying client settings in channels (including private channels).
- Added a new CopyFrom (DiscordVoiceStream method) overload with a file path as a parameter, serving as a shortcut for DiscordVoiceUtils.ReadFromFile().

### Improvements
- Pulling guild members has been improved. GetGuildMembers is for bots while GetGuildChannelMembers is for users. This is due to Discord making some changes to their API.
- DiscordImages are now used when inputting images.
- Rewrote how permissions work.
- Replaced GuildMember's 'HasPermission' method with 'GetPermissions' to increase speeds.



# 0.7.0.2
### Additions
- Added the newest messagetypes to MessageType.

### Improvements
- Updated various methods and properties in the following classes: Group, DeletedMessage, MessageReactionUpdate, UserTyping.



# 0.7.0.0
### Additions
- Added support for speaking through voice channels using a DiscordVoiceSession (even in private channels).
- Added some caching functionality to help speed up applications. Most of these functions have the same name as their REST equivalent but require a DiscordSocketClient (except for Get[Guild/PrivateChannel]VoiceStates, which are new methods alltogether).
- Added configs to the clients, allowing you to do things like RetryOnRateLimited, which will handle ratelimits so you don't have to.
- Added support for guild templating.
- Added more auth support.
- Added support for sending files through messages (SendFile).
- Added some more oauth2 application functionality.
- Added support for manipulating guild folders through User.ChangeSettings().
- Added some more events to DiscordSocketClient.
- Added functionality for buying nitro/boosts.

### Improvements
- Changed the way image hashes are dealt with. AvatarId, IconId etc. no longer exist, neither does their 'Get' methods. They've been replaced with a DiscordCDNImage instance (containg a Download method). Check source for more info.
- Renamed some classes to their original names prepended with 'Discord' to avoid collision with other libraries.
- Made changes to how command handling classes are structured. See example project 'CommandListener' for details.
- Improved the way Invites and Presences (therefore also Activities) are represented in Anarchy.



# 0.6.5.1
### Additions
- Added some connections functionality.

### Improvements
- Fixed the CommandHandler's command dispatcher.

# 0.6.5.0
### Additions
- Added a command handler (can only be used by a DiscordSocketClient). Explaining how it works is too much here, so i have added an example project for it.
- Added "minimalistic" models which help speed up various parts of your bot.

###Improvements
- Removed methods previously marked obsolete.



# 0.6.4.0

### Additions
- Added proxy support.
- Added support for setting custom statuses.
- Added server discovery functionality.
- Added the ability to disable anti-track.
- A bunch of other shit i can't remember, lol.

### Improvements
- when converting objects that have multiple types (like channels), instead of needing to make another HTTP request - it saves the json from the request and deserializes to that if asked.
- Improved the Settings class (used in OnSettingsUpdated).



# 0.6.3.1

### Additions
- Added implicit convertions for objects with IDs.
- Added event OnProfileUpdated.

### Improvements
- Many other minor improvements.



# 0.6.3.0

### Additions
- Added nitro boost support.
- Added nitro gift support.
- Added more gateway events (for an example OnSettingsUpdated).
- Added support for setting images to base64 format at constructor.

# 0.6.2.2

### Additions
- Added functions for acknowledging messages (AcknowladgeGuildMessages/AcknowladgeMessage).
- Added support for detecting CF rate limits (RateLimitExceptions with a RetryAfter of 0).
- Did a few name changes for avoiding having same names as other libs.

# 0.6.2.1

### Additions
- Added OnUserUpdated event.
- Added user-only gateway events.
- Added Local to UserSettings allowing you to change the client's language.
- Added methods to Guild for getting members.
- Added functionality for getting/changing more personal settings.

### Improvements
- client.User.Modify() was changed to ChangeProfile() because of new settings functions.




# 0.6.2.0

### Additions
- Re-added GetInvite() method.
- Added some OAuth2 support.
- Added Type in Invite, telling the user if it's for a guild or a group.
- Added EmbedException to prevent sending invalid embeds.

### Improvements
- Changed guild channel creation stuff (sorry for name changes).
- Improved Connections functionality.
- Fixed connection resuming (like actually this time).
- Other minor bug fixes.



# 0.6.1.0

### Additions
- Added Badges to user (also with a new Hypesquad property).
- Added Nitro to Profile.
- Added a few more errors to DiscordError.

### Improvements
- client.CreateGuild() now no longer takes in a 'GuildCreationProperties', but instead takes in it's members.
- PartialGuild now also supports modifying and updating.
- Minor code readability improvements and bug fixes.



# 0.6.0.0

### Additions
- Added more gateway events.
- Added some groundlaying voice functionality (more coming soon :)).
- Added support for modifying guild members further than their nickname.
- Added options for filtering messages.
- Added support for deleting and disabling accounts.
- Added more user-related DiscordErrors.

### Improvements
- Modified many models to not be dependant on the containing model's info, making them a lot more secure.
- Split Invite up into GroupInvite and GuildInvite.
- Extended Activity to GameActivity and StreamActivity (Watching and Listening still use listening). Since has now also been renamed to Elapsed.
- Fixed the client logging out whenever a password is being set.
- Removed ActivitiyAssets because of them being unusable.
- There have also been a lot of name changes to methods and models (sorry, i'll try to keep those low from now on).

### Bug fixes
- DiscordSocketClient is now able to resume dropped connections.
- Other minor bug fixes.



# 0.3.0.1
- Added 'Watching' to ActivityType.
- Fixed 'since' parameter in SetActivity().
- Minor bug fixes.



# 0.3.0.0

### Additions
- Added support for DM channels.
- Added GetProfile() and GetMutualFriends().
- Added comments throughout the code to document what the different classes/methods do.
- Added more methods to a bunch of models.
- Added more gateway events.

### Improvements
- Improved security and failsafety.
- Changed many exceptions to be more beginner friendly.
- Fixed PartialEmoji, Emoji and MessageReaction having values that never get set.
- Timestamps are now DateTime objects instead of raw strings.
- Improved sending messages through webhooks (you might have to change your bot).

### Bug fixes
- Fixed GetChannelMessages() getting practically random messages.
- Other minor bug fixes.

### Removels
- Removed MessageProperties overload for SendMessage(), making it look nicer (you might have to change ur bot).
<br><br><br>



# 0.1.2.0

### Additions
- Added a temporary solution to the 'messages in voice channels' problem.
- Added more user-account based values in DiscordError.

### Improvements
- Changed PartialGuild to fit in invite responses.
- Improved GuildId populating.
- Changed many int and long property types to restrict them from being negative.
- Updated GuildDuplicator to match newest version.

### Bug fixes
- Minor bug fixes.



# 0.1.1.1
- Added enum for DiscordHttpError's 'Code' property (DiscordError)
- Added more gateway events (such as OnUserBanned and OnEmojisUpdated)
- Minor reorganizations.
<br><br><br>



# 0.1.1.0

### Additions
- Added embed support for webhooks.
- Added support for all different channels.
- Added permission overwrite functionality.
- Added PartialInvite because of missing information at some endpoints.

### Improvements
- Improved relationship functionality.
- Improved ClientUser funcionality.
- Improved HTTP error handling.

### Bug fixes
- A bunch of minor bug fixes.
<br><br><br>



# 0.1.0.0 [STABLE]

### Additions
- Added GetAvatar()/GetIcon() methods, allowing you to download images from guilds n' stuff.
- Added PermissionCalculator.Create(), which allows you to create EditablePermissions objects from lists of permissions.
- Added EmbedMaker creating and sending embeds.
- Implemented IReadOnlyList<T> for more security.
- Added a new Type flag in User for identying users.
- Added activities for DiscordSocketClient.

### Changes
- Changed 'Reaction' to 'Emoji' where it is appropriate.
- Color property in Role and Embed are now actual Color objects, and not int values.

### Bug fixes
- Added an Update() method for ClientUser.
- Improved 'default properties' fix by implementing a new Property<T> class.
<br><br><br>



# 0.1.0.0 [BETA]

### Additions
- Added a much quicker way of getting guild members (tho since it's using the gateway it only supports DiscordSocketClient).
- Added a permission calculator (which has currently not been incorporated into the managable models, but you can still use PermissionCalculator).
- Added audit log functionality.
- Update() methods for all managable Discord models have been added, allowing you to update the local information whenever you want.
- Added multi image format support, meaning that you can now use jpg, png, and gif files.
- Added 'partial types' that get used when Discord does not respond with full ones.
- Completed list of gateway opcodes.

### Changes
- DiscordWebhookClient has been depricated. Hook (webhook object that depends on a DiscordClient) now holds the data as well as basically being the new DiscordWebhookClient.
- Improved object code organization.
- Reorganized files and folders.

### Bug fixes
- Fixed Properties classes setting properties that are not set (or 'null').
<br><br><br>



# 0.0.1.1
#### Additions
- Added AddChannelPermissionOverwrite() (please keep in mind tho that a bit calculator for permissions has not been added yet).
- Added InvalidParametersException. When invalid parameters are passed in, it making it harder for the wrapper to crash unexpectedly. This has been applied to DiscordHttpClient.
- Added Modification capability for reactions.
- Added ability to unfriend someone.

#### Bug fixes
- Fixed clients and guild id's not being passed into reactions and roles when downloading guilds.
- Fixed modification for channels and roles.
- Fixed message properties not being readonly.
- Other minor bug fixes.

#### Other changes
- ChannelProperties has been split into ChannelCreationProperties and ChannelModProperties since you're able to change more when modifying.
<br><br><br>



# 0.0.1.0
- Initial release.
