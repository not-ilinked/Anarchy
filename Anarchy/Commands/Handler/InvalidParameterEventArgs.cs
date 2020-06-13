namespace Discord.Commands
{
    public class InvalidParameterEventArgs : MissingParameterEventArgs
    {
        public string InputtedParameter { get; private set; }


        public InvalidParameterEventArgs(DiscordCommand command, ParameterAttribute param, string input) : base(command, param)
        {
            InputtedParameter = input;
        }
    }
}
