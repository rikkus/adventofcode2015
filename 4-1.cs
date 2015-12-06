var md5 = System.Security.Cryptography.MD5.Create();
​
Enumerable.Range(1, 999999999).First(i => {
		if (i % 1000000 == 0)
			Console.WriteLine(i/1000000 + " million");
		
		return BitConverter
			.ToString(md5.ComputeHash(System.Text.ASCIIEncoding.ASCII.GetBytes("ckczppom" + i.ToString())))
			.Replace("-", "")
			.StartsWith("00000");
	}
).Dump();