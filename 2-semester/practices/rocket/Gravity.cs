namespace func_rocket;

/// <param name="location">
/// находится в пределах прямоугольника (0, 0) — (spaceSize.Width, spaceSize.Height)
/// </param>
public delegate Vector Gravity(Vector spaceSize, Vector location);