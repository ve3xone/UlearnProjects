static int Decode(string message)
{
	return int.Parse(new string(message.ToCharArray().Where(x => "0123456789".Contains(x)).ToArray())) % 1024;
}