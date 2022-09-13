// See https://aka.ms/new-console-template for more information
using System.Text;

LongNumber A = new LongNumber();
LongNumber B = new LongNumber();
A.value = "-1999996162472472525";
B.value = "-1999996162472472525";
StringBuilder str = new StringBuilder("572572572", 100);
//LongNumber C = A / B;
Console.WriteLine(str.ToLongNumber());


public class LongNumber
{
	public string value;

	public LongNumber()
	{
		value = "0";

	}

	public LongNumber(string value)
	{
		this.value = value;
	}

	public void Copy(LongNumber ln)
	{
		value = ln.value;
	}

	public override bool Equals(object? obj)
	{
		if (obj == null) return false;
		if (!(obj is LongNumber)) return false;
		LongNumber ln = (LongNumber)obj;
		return value == ln.value;
	}

	public override int GetHashCode()
	{
		return value.GetHashCode();
	}

	public override string? ToString()
	{
		return value;
	}

	public static LongNumber operator +(LongNumber ln1, LongNumber ln2)
	{
		LongNumber ln11 = new LongNumber();
		ln11.Copy(ln1);
		LongNumber ln22 = new LongNumber();
		ln22.Copy(ln2);
		bool minusbefore = false;
		if (ln11.value[0] == '-' && ln22.value[0] == '-') // если два отрицательных, то превращаем их в положительные и в конце добавляем к результату минус
		{
			minusbefore = true;
			ln11.value = ln11.value.Substring(1);
			ln22.value = ln22.value.Substring(1);
		}
		else if (ln11.value[0] == '-' && ln22.value[0] != '-') // если одно отрицательное, то убираем у него минус и вычитаем его из положительного
		{
			ln11.value = ln11.value.Substring(1);
			return ln22 - ln11;
		}
		else if (ln11.value[0] != '-' && ln22.value[0] == '-')
		{
			ln22.value = ln22.value.Substring(1);
			return ln11 - ln22;
		}
		ln11.value = ln11.value.TrimStart(new char[] { '0' });
		ln22.value = ln22.value.TrimStart(new char[] { '0' });
		int ln1len = ln11.value.Length;
		int ln2len = ln22.value.Length;
		int minlength = Math.Min(ln1len, ln2len); // находим длину кратчайшего числа
		int temp, flag = 0, i;
		string result = String.Empty;
		for (i = 0; i < minlength; i++) // конвертим посимвольно в инт с конца, складываем и добавлем в начало результата
		{
			temp = Convert.ToInt32(ln11.value[ln1len - i - 1].ToString()) + Convert.ToInt32(ln22.value[ln2len - i - 1].ToString()) + flag;
			if (temp >= 10)
			{
				temp -= 10;
				flag = 1;
			}
			else flag = 0;
			result = Convert.ToString(temp) + result;
		}
		string longstr; // находим длинейшее число
		if (ln1len > ln2len)
			longstr = ln11.value;
		else longstr = ln22.value;
		int maxlength = Math.Max(ln1len, ln2len);
		while (flag != 0 && i != maxlength) // продолжаем добавлять перенос десятка после того, как зокнчилось второе число
		{
			temp = Convert.ToInt32(longstr[maxlength - i - 1].ToString()) + flag;
			if (temp >= 10)
			{
				temp -= 10;
				flag = 1;
			}
			else flag = 0;
			result = Convert.ToString(temp) + result;
			i++;
		}
		if (i == maxlength && flag != 0) // если и это число закончилось, а перенос всё ещё есть, то добавляем единицу в начало и возвращаем значение
		{
			result = "1" + result;
			LongNumber finalres = new LongNumber();
			if (minusbefore)
				finalres.value = "-" + result;
			else finalres.value = result;
			return finalres;
		}
		else if (i == maxlength) // если число закончилось, а переноса нет, то возвращаем значение
		{
			LongNumber finalres = new LongNumber();
			if (minusbefore && result != "0")
				finalres.value = "-" + result;
			else finalres.value = result;
			return finalres;
		}
		else
		{
			string tempstr = longstr.Substring(0, longstr.Length - i);
			result = tempstr + result;
			LongNumber finalres = new LongNumber();
			if (minusbefore && result != "0")
				finalres.value = "-" + result;
			else finalres.value = result;
			return finalres;
		}
	}
	public static LongNumber operator -(LongNumber ln1, LongNumber ln2)
	{
		LongNumber ln11 = new LongNumber();
		ln11.Copy(ln1);
		LongNumber ln22 = new LongNumber();
		ln22.Copy(ln2);
		bool minusbefore = false;
		if (ln11.value[0] == '-' && ln22.value[0] != '-') // если уменьшаемое отриц, а вычитаемое полож, то делаем сложение двух отриц чисел
		{
			ln22.value = "-" + ln22.value;
			return ln11 + ln22;
		}
		else if (ln11.value[0] != '-' && ln22.value[0] == '-') // если уменьшаемое полож, а вычитаемое отриц, то сложение двух положительных чисел
		{
			ln22.value = ln22.value.Substring(1);
			return ln11 + ln22;
		}
		else if (ln11.value[0] == '-' && ln22.value[0] == '-' && ln11.value.Length > ln22.value.Length)
		{
			ln11.value = ln11.value.Substring(1);
			ln22.value = ln22.value.Substring(1);
			minusbefore = true;
		}
		else if (ln11.value.Length < ln22.value.Length) // делаем так, чтоб первое число было больше второго
		{
			LongNumber temp1 = ln11;
			ln11 = ln22;
			ln22 = temp1;
			if (ln11.value[0] == '-')
			{
				ln11.value = ln11.value.Substring(1);
				ln22.value = ln22.value.Substring(1);
				minusbefore = false;
			}
			else minusbefore = true;
		}
		else if (ln11.value.Length == ln22.value.Length)
		{
			int j = 0;
			while (j != ln11.value.Length && ln11.value[j] == ln22.value[j])
				j++;
			if (j != ln11.value.Length && ln11.value[j] < ln22.value[j])
			{
				LongNumber temp1 = ln11;
				ln11 = ln22;
				ln22 = temp1;
				if (ln11.value[0] == '-')
				{
					ln11.value = ln11.value.Substring(1);
					ln22.value = ln22.value.Substring(1);
					minusbefore = false;
				}
				else minusbefore = true;
			}
		}
		ln11.value = ln11.value.TrimStart(new char[] { '0' });
		ln22.value = ln22.value.TrimStart(new char[] { '0' });
		int ln1len = ln11.value.Length;
		int ln2len = ln22.value.Length;
		//int minlength = Math.Min(ln1len, ln2len); // находим длину кратчайшего числа
		int temp, flag = 0, i;
		string result = String.Empty;
		for (i = 0; i < ln2len; i++) // конвертим посимвольно в инт с конца, вычитаем и добавлем в начало результата
		{
			temp = Convert.ToInt32(ln11.value[ln1len - i - 1].ToString()) - Convert.ToInt32(ln22.value[ln2len - i - 1].ToString()) - flag;
			if (temp < 0)
			{
				temp += 10;
				flag = 1;
			}
			else flag = 0;
			result = Convert.ToString(temp) + result;
		}
		while (flag != 0 && i != ln1len) // продолжаем добавлять перенос десятка после того, как закончилось второе число
		{
			temp = Convert.ToInt32(ln11.value[ln1len - i - 1].ToString()) - flag;
			if (temp < 0)
			{
				temp += 10;
				flag = 1;
			}
			else flag = 0;
			result = Convert.ToString(temp) + result;
			i++;
		}
		if (i == ln1len) // если число закончилось, то возвращаем значение
		{
			LongNumber finalres = new LongNumber();
			if (minusbefore && result != "0")
				finalres.value = "-" + result;
			else finalres.value = result;
			return finalres;
		}
		else
		{
			string tempstr = ln11.value.Substring(0, ln1len - i);
			if (minusbefore && result != "0")
				result = "-" + tempstr + result;
			else result = tempstr + result;
			LongNumber finalres = new LongNumber();
			finalres.value = result;
			return finalres;
		}
	}
	public static LongNumber operator *(LongNumber ln1, LongNumber ln2)
	{
		LongNumber ln11 = new LongNumber();
		ln11.Copy(ln1);
		LongNumber ln22 = new LongNumber();
		ln22.Copy(ln2);
		bool minusbefore = false;
		if (!((ln11.value[0] == '-' && ln22.value[0] == '-') || (ln11.value[0] != '-' && ln22.value[0] != '-')))
		{
			minusbefore = true;
			if (ln11.value[0] == '-')
			{
				ln11.value = ln11.value.Substring(1);
			}
			if (ln22.value[0] == '-')
			{
				ln22.value = ln22.value.Substring(1);
			}
		}
		else if (ln11.value[0] == '-')
		{
			ln11.value = ln11.value.Substring(1);
			ln22.value = ln22.value.Substring(1);
		}
		ln11.value = ln11.value.TrimStart(new char[] { '0' });
		ln22.value = ln22.value.TrimStart(new char[] { '0' });
		int i, j, ln2len = ln22.value.Length;
		LongNumber temp = new LongNumber();
		LongNumber result = new LongNumber();
		result.value = "0";
		for (i = 0; i < ln22.value.Length; i++)
		{
			temp.value = "0";
			int mult = Convert.ToInt32(ln22.value[ln2len - i - 1].ToString()); // на сколько надо умножить
			for (j = 0; j < mult; j++) // столько раз прибавляем
			{
				temp = temp + ln11;
			}
			for (j = 0; j < i; j++)
			{
				temp.value = temp.value + "0"; // прибавляем нули в конец в зависимости от разряда
			}
			result = result + temp;
		}
		if (minusbefore && result.value != "0")
		{
			result.value = "-" + result.value;
			return result;
		}
		else return result;
	}
	public static LongNumber operator /(LongNumber ln1, LongNumber ln2)
	{
		LongNumber ln11 = new LongNumber();
		ln11.Copy(ln1);
		LongNumber ln22 = new LongNumber();
		ln22.Copy(ln2);
		bool minusbefore = false;
		if (!((ln11.value[0] == '-' && ln22.value[0] == '-') || (ln11.value[0] != '-' && ln22.value[0] != '-')))
		{
			minusbefore = true;
			if (ln11.value[0] == '-')
			{
				ln11.value = ln11.value.Substring(1);
			}
			if (ln22.value[0] == '-')
			{
				ln22.value = ln22.value.Substring(1);
			}
		}
		else if (ln11.value[0] == '-')
		{
			ln11.value = ln11.value.Substring(1);
			ln22.value = ln22.value.Substring(1);
		}
		ln11.value = ln11.value.TrimStart(new char[] { '0' }); // обрезаем нули в начале
		ln22.value = ln22.value.TrimStart(new char[] { '0' });
		int ln1len = ln11.value.Length;
		int ln2len = ln22.value.Length;
		int i, j;
		for (i = 0; i < ln1len - ln2len; i++) // добавляем столько нулей делителю, чтоб был одной длины с уменьшаемым
		{
			ln22.value += "0";
		}
		LongNumber result = new LongNumber();
		result.value = String.Empty;
		for (i = 0; i < ln1len - ln2len + 1; i++) // прогоняем столько раз, сколько нулей мы добавили + 1
		{
			int mult = -1; // промежуточное частное
			while (ln11.value[0] != '-')
			{
				ln11 = ln11 - ln22;
				mult++;
			}
			ln11 = ln11 + ln22;
			ln22.value = ln22.value.Substring(0, ln22.value.Length - 1); // убираем 1 нуль с конца
			result.value += Convert.ToString(mult); // добавляем промежутчное частное в конец результата
		}
		result.value = result.value.TrimStart(new char[] { '0' }); // обрезаем нули в начале
		if (result.value == "")
			result.value = "0";
		if (minusbefore && result.value != "0")
		{
			result.value = "-" + result.value;
			return result;
		}
		else return result;
	}

	public static bool operator ==(LongNumber ln1, LongNumber ln2)
	{
		if (ln1.value == ln2.value)
			return true;
		else return false;
	}
	public static bool operator !=(LongNumber ln1, LongNumber ln2)
	{
		if (ln1.value != ln2.value)
			return true;
		else return false;
	}

	public static explicit operator int(LongNumber ln)
	{
		try
		{
			int a = Convert.ToInt32(ln.value);
			return a;
		}
		catch (OverflowException)
		{
			return 0;
		}

	}
	public static explicit operator long(LongNumber ln)
	{
		try
		{
			long a = Convert.ToInt64(ln.value);
			return a;
		}
		catch (OverflowException)
		{
			return 0;
		}

	}
	public static explicit operator short(LongNumber ln)
	{
		try
		{
			short a = Convert.ToInt16(ln.value);
			return a;
		}
		catch (OverflowException)
		{
			return 0;
		}

	}
	public static explicit operator bool(LongNumber ln)
	{
		int a;
		try
		{
			a = Convert.ToInt32(ln.value);
		}
		catch (OverflowException)
		{
			a = 1;
		}
		return Convert.ToBoolean(a);
	}

	public static implicit operator LongNumber(int num)
	{
		LongNumber ln = new LongNumber(Convert.ToString(num));
		return ln;
	}
	public static implicit operator LongNumber(long num)
	{
		LongNumber ln = new LongNumber(Convert.ToString(num));
		return ln;
	}
	public static implicit operator LongNumber(short num)
	{
		LongNumber ln = new LongNumber(Convert.ToString(num));
		return ln;
	}
	public static implicit operator LongNumber(bool num)
	{
		LongNumber ln = new LongNumber();
		if (num)
			ln.value = "1";
		else ln.value = "0";
		return ln;
	}

	/*public static bool TryParse(this string str, out LongNumber ln)
	{
		bool flag = true;
		int i = 0;
		if (str[0] == '-')
			i++;
		else
		{
			while (i != str.Length)
				if (!Char.IsNumber(str, i))
				{
					flag = false;
					break;
				}
				i++;
        }
		if (flag)
		{
			ln = new LongNumber(str);
			return true;
		}
		return bool.TryParse(str, out ln);
    } */

}

public static class StringExtension
{
	public static LongNumber ToLongNumber(this string str)
	{
		LongNumber ln;
		int i = 0, a;
		if (str[0] == '-')
			i++;
		try
		{
			while (i != str.Length)
			{
				a = Convert.ToInt32(str[i].ToString());
				i++;
			}
			ln = new LongNumber(str);
			return ln;
		}
		catch (FormatException) // как сделать, чтобы прерывалось выполнение, если строка не подходит???
		{
			Console.WriteLine($"Unable to parse '{str}'");
		}
		return new LongNumber();
	}

	public static LongNumber ToLongNumber(this StringBuilder str)
	{
		LongNumber ln;
		int i = 0, a;
		if (str[0] == '-')
			i++;
		try
		{
			while (i != str.Length)
			{
				a = Convert.ToInt32(str[i].ToString());
				i++;
			}
			ln = new LongNumber(str.ToString());
			return ln;
		}
		catch (FormatException) // как сделать, чтобы прерывалось выполнение, если строка не подходит???
		{
			Console.WriteLine($"Unable to parse '{str}'");
		}
		return new LongNumber();
	}
}


