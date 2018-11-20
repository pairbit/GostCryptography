﻿using System.Security;

using GostCryptography.Asn1.Gost.Gost_R3410_2001;
using GostCryptography.Native;

namespace GostCryptography.Gost_R3410
{
	/// <inheritdoc />
	public sealed class Gost_R3410_2001_KeyExchangeAlgorithm : Gost_R3410_KeyExchangeAlgorithm
	{
		/// <inheritdoc />
		[SecurityCritical]
		public Gost_R3410_2001_KeyExchangeAlgorithm(int providerType, SafeProvHandleImpl provHandle, SafeKeyHandleImpl keyHandle, Gost_R3410_2001_KeyExchangeParams keyExchangeParameters)
			: base(providerType, provHandle, keyHandle, keyExchangeParameters)
		{
		}
	}
}