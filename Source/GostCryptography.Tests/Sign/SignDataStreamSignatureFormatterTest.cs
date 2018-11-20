﻿using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

using GostCryptography.Base;
using GostCryptography.Gost_R3411;

using NUnit.Framework;

namespace GostCryptography.Tests.Sign
{
	/// <summary>
	/// Подпись и проверка подписи потока байт с помощью сертификата и классов формтирования.
	/// </summary>
	/// <remarks>
	/// Тест создает поток байт, вычисляет цифровую подпись потока байт с использованием закрытого ключа сертификата,
	/// а затем с помощью открытого ключа сертификата проверяет полученную подпись. Для вычисления цифровой подписи
	/// используется класс <see cref="GostSignatureFormatter"/>, для проверки цифровой подписи используется класс
	/// <see cref="GostSignatureDeformatter"/>.
	/// </remarks>
	[TestFixture(Description = "Подпись и проверка подписи потока байт с помощью сертификата и классов формтирования")]
	public sealed class SignDataStreamSignatureFormatterTest
	{
		[Test]
		public void ShouldSignDataStream()
		{
			// Given
			var certificate = TestCertificates.GetCertificate();
			var privateKey = (GostAsymmetricAlgorithm)certificate.GetPrivateKeyAlgorithm();
			var publicKey = (GostAsymmetricAlgorithm)certificate.GetPrivateKeyAlgorithm();
			var dataStream = CreateDataStream();

			// When

			dataStream.Seek(0, SeekOrigin.Begin);
			var signature = CreateSignature(privateKey, dataStream);

			dataStream.Seek(0, SeekOrigin.Begin);
			var isValidSignature = VerifySignature(publicKey, dataStream, signature);

			// Then
			Assert.IsTrue(isValidSignature);
		}

		private static Stream CreateDataStream()
		{
			// Некоторый поток байт для подписи

			return new MemoryStream(Encoding.UTF8.GetBytes("Some data for sign..."));
		}

		private static byte[] CreateSignature(GostAsymmetricAlgorithm privateKey, Stream dataStream)
		{
			byte[] hash;

			using (var hashAlg = new Gost_R3411_94_HashAlgorithm())
			{
				hash = hashAlg.ComputeHash(dataStream);
			}

			var formatter = new GostSignatureFormatter(privateKey);

			return formatter.CreateSignature(hash);
		}

		private static bool VerifySignature(GostAsymmetricAlgorithm publicKey, Stream dataStream, byte[] signature)
		{
			byte[] hash;

			using (var hashAlg = new Gost_R3411_94_HashAlgorithm())
			{
				hash = hashAlg.ComputeHash(dataStream);
			}

			var deformatter = new GostSignatureDeformatter(publicKey);

			return deformatter.VerifySignature(hash, signature);
		}
	}
}