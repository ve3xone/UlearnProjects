static bool ShouldFire2(bool enemyInFront, string enemyName, int robotHealth)
{
    return ((enemyInFront == true) && (enemyName == "boss") && (robotHealth >= 50))||      ((enemyInFront == true) && (enemyName != "boss"));
}