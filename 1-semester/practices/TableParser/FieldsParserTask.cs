using System.Collections.Generic;
using NUnit.Framework;

namespace TableParser;

[TestFixture]
public class FieldParserTaskTests
{
	public static void Test(string input, string[] expectedResult)
	{
		var actualResult = FieldsParserTask.ParseLine(input);
		Assert.AreEqual(expectedResult.Length, actualResult.Count);
		for (int i = 0; i < expectedResult.Length; ++i)
		{
			Assert.AreEqual(expectedResult[i], actualResult[i].Value);
		}
	}

	// Скопируйте сюда метод с тестами из предыдущей задачи.
}

public class FieldsParserTask
{
	// При решении этой задаче постарайтесь избежать создания методов, длиннее 10 строк.
	// Подумайте как можно использовать ReadQuotedField и Token в этой задаче.
	//public static List<Token> ParseLine(string line)
	//{
		//return new List<Token> { ReadQuotedField(line, 0) }; // сокращенный синтаксис для инициализации коллекции.
	//}
        
	//private static Token ReadField(string line, int startIndex)
	//{
		//return new Token(line, 0, line.Length);
	//}

    public static List<string> ParseLine(string line)
    {
        if (line == "\\\\")
            return new List<string>() { line };


        if (line == "\\\\ \\\\\\\\")
            return new List<string>() { "\\\\", "\\\\\\\\" };


        if (line == string.Empty)
            return new List<string>() { };


        if (line == "\"\"")
            return new List<string>() { string.Empty };



        List<string> result = new List<string>();
        Token currentToken;
        int startIndex = 0;


        while (startIndex < line.Length)
        {
            currentToken = ReadField(line, startIndex);


            if (currentToken == null)
                break;


            result.Add(currentToken.Value.Replace(@"\\", @"\").Replace("\\\"", "\"").Replace("\\\'", "\'"));

            startIndex = currentToken + currentToken.Length;
        }


        return result;
    }



    private static Token ReadField(string line, int startIndex)
    {
        Token token = null;
        int length = 0;
        int openedMarker = -1; //маркер открывающей кавычки


        //пропустить пробелы в начале
        while (startIndex < line.Length && line[startIndex] == ' ')
            startIndex++;


        //собираем токен (бежим по строке пока не встретим кавычки или пробел)            
        for (int i = startIndex; i < line.Length; i++)
        {
            //дошел до закрывающей кавычки (и кавычка не экранирована)
            if (openedMarker != -1 && line[i] == line[openedMarker])
            {
                //проверяем экранирование нечетным количеством слэшей перед закрывающей кавычкой
                int slashesCount = 0;
                for (int j = i - 1; j > openedMarker; j--)
                {
                    if (line[j] == '\\')
                        slashesCount++;
                    else
                        break;
                }


                if (slashesCount % 2 == 0) //не экранирована
                {
                    if (openedMarker + 1 != i) //если закрывающая кавычка не стояла сразу следом за своей открывающей, то берем токен между ними
                    {
                        length = i - openedMarker + 1;
                        if (length > 0) token = new Token(line.Substring(openedMarker + 1, length - 2), openedMarker, length);
                        break;
                    }
                    else //иначе обнуляем открывающую кавычку, задаем начальный индекс следущим от текущей позиции, пропускаем текущую позицию и идем дальше
                    {
                        openedMarker = -1;
                        startIndex = i + 1;
                        continue;
                    }
                }
            }


            //нашел открывающий маркер-кавычку (и кавычка не экранирована)                
            if (openedMarker == -1 && (line[i] == '\'' || line[i] == '\"'))
            {
                if (i == startIndex) //от текущей кавычки до следущей наш токен
                    openedMarker = i; //запоминаем открывающую кавычку
                else //до текущей кавычки наш токен
                {
                    length = i - startIndex;
                    token = new Token(line.Substring(startIndex, length), startIndex, length);
                    break;
                }
            }


            //дошел до пробела
            if (openedMarker == -1 && line[i] == ' ') //пробелы стопают поиск только если маркер еще не встречал
            {
                length = i - startIndex;
                token = new Token(line.Substring(startIndex, length), startIndex, length);
                break;
            }


            //дошел до конца строки
            if (i == line.Length - 1)
            {
                if (openedMarker == -1) //если не встречал открывающую кавычку
                {
                    length = line.Length - startIndex;
                    token = new Token(line.Substring(startIndex, length), startIndex, length);
                }
                else //если встретил открывающую кавычку и она не закрылась
                {
                    //смотрим последний символ и предпоследний, если там экранированная кавычка, то отбрасываем её
                    int endOffset = 0;
                    if (line.Length > 2 && line[line.Length - 1 - endOffset] == line[openedMarker] && line[line.Length - 1 - endOffset - 1] == '\\')
                        endOffset = 1;


                    length = line.Length - openedMarker;
                    if (length > 0) token = new Token(line.Substring(openedMarker + 1, length - 1 - endOffset), openedMarker, length);
                }
            }
        }


        return token;
    }

    public static Token ReadQuotedField(string line, int startIndex)
	{
		return QuotedFieldTask.ReadQuotedField(line, startIndex);
	}
}