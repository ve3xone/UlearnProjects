public static GameResult GetGameResult(Mark[,] field)
{
    if (HasWinSequence(field, Mark.Cross)&&HasWinSequence(field, Mark.Circle))
        return GameResult.Draw;
    if (HasWinSequence(field, Mark.Cross))
        return GameResult.CrossWin;
    if (HasWinSequence(field, Mark.Circle))
        return GameResult.CircleWin;
    else return GameResult.Draw;
}

static bool HasWinSequence(Mark[,] field, Mark mark)
{
    bool line = HasInHorizontalLine(field, mark);
    bool column = HasInVerticalLine(field, mark);
    bool diagonal = HasInDiagonalLine(field, mark);
   
    return line||column||diagonal;
}

static bool HasInHorizontalLine(Mark[,]field, Mark mark)
{
    for (int i = 0; i < 3; i++)
    {
        for (int j = 1; j < 2; j++)
        {
            if (field[i, j - 1] == mark && field[i, j] == mark && field[i, j + 1] == mark)
                return true;
        }
    }
    return false;
}

static bool HasInVerticalLine(Mark[,] field, Mark mark)
{
    for (int i = 0; i < 3; i++)
    {
        for (int j = 1; j < 2; j++)
        {
            if (field[j - 1, i] == mark && field[j, i] == mark && field[j + 1, i] == mark)
                return true;
        }
    }
    return false;
}

static bool HasInDiagonalLine(Mark[,] field, Mark mark)
{
    return (field[0, 0] == mark && field[1, 1] == mark && field[2, 2] == mark) ||
       	   (field[2, 0] == mark && field[1, 1] == mark && field[0, 2] == mark);
}