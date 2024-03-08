public static bool IsCorrectMove(string from, string to)
{
    var dx = Math.Abs(to[0] - from[0]); //смещение фигуры по горизонтали
	var dy = Math.Abs(to[1] - from[1]); //смещение фигуры по вертикали

	//ферзь остается на месте
    if (dx == 0 && dy == 0)
		return false;

	//проверяем, что ферзь движется по прямой или по диагонали
    if (dx == 0 || dy == 0 || dx == dy) 
        return true;
	
	//если ферзь движется не по прямой и не по диагонали, то ход некорректный
    return false; 
}