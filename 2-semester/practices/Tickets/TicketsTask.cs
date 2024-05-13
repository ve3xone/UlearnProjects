using System.Numerics;

namespace Tickets;

public class TicketsTask
{
    public static BigInteger Solve(int ticketLength, int ticketSum)
    {
        if (ticketSum % 2 != 0) return 0;
        var halfSum = ticketSum / 2;
        var count = new BigInteger[ticketLength + 1, halfSum + 1];

        for (int i = 0; i <= ticketLength; i++)
            count[i, 0] = 1;

        for (int i = 1; i <= ticketLength; i++)
            for (int j = 1; j <= halfSum; j++)
                for (int n = 0; n <= 9 && j - n >= 0; n++)
                    count[i, j] += count[i - 1, j - n];

        return count[ticketLength, halfSum] * count[ticketLength, halfSum];
    }
}