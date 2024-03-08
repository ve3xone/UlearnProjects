namespace yield;

public class Factory
{
	public static IDataAnalyzer CreateAnalyzer()
	{
		//return new checking.DataAnalyzerSolvedImpl();
		return new DataAnalyzerImpl();
	}
}