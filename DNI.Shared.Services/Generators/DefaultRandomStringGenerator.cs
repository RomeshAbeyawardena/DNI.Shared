﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using DNI.Shared.Contracts.Generators;
using DNI.Shared.Contracts.Enumerations;
using DNI.Shared.Contracts;

namespace DNI.Shared.Services.Generators
{
    internal sealed class DefaultRandomStringGenerator : IRandomStringGenerator
    {
        private readonly RandomNumberGenerator _randomNumberGenerator;
        private readonly ISwitch<CharacterType, Range> _rangeSwitch;
        public DefaultRandomStringGenerator(RandomNumberGenerator randomNumberGenerator)
        {
            _randomNumberGenerator = randomNumberGenerator;
            _rangeSwitch = Switch.Create<CharacterType, Range>()
                .CaseWhen(CharacterType.Lowercase, new Range(97, 122))
                .CaseWhen(CharacterType.Uppercase, new Range(65, 90))
                .CaseWhen(CharacterType.Numerics, new Range(48, 57))
                .CaseWhen(CharacterType.Symbols, new Range(33, 47));
        }

        public string GenerateString(CharacterType characterType, int length)
        {
            return string.Concat(GetCharacters(characterType, length));
        }

        private IEnumerable<char> GetCharacters(CharacterType characterType, int length)
        {
            var ranges = GetCharacterRanges(characterType).ToArray();
            IEnumerable<int> rangeSequence = Array.Empty<int>();

            foreach(Range range in ranges)
                rangeSequence = rangeSequence.Concat(GetSequence(range));
            
            var buffer = new byte[256 * ranges.Length];

            _randomNumberGenerator.GetBytes(buffer); 

            return buffer.ToArray()
                .Where(index => rangeSequence.Any(rangeIndex => rangeIndex.Equals(index)))
                .Take(length).Select(b => (char)b);
        }

        private IEnumerable<int> GetSequence(Range range)
        {
            var rangeList = new List<int>();
            for (var index = range.Start.Value; index <= range.End.Value; index++)
                rangeList.Add(index);

            return rangeList.ToArray();
        }

        private IEnumerable<Range> GetCharacterRanges(CharacterType characterType)
        {
            var characterRangeList = new List<Range>();
            if (characterType.HasFlag(CharacterType.Lowercase))
                characterRangeList.Add(_rangeSwitch.Case(CharacterType.Lowercase));

            if (characterType.HasFlag(CharacterType.Uppercase))
                characterRangeList.Add(_rangeSwitch.Case(CharacterType.Uppercase));

            if (characterType.HasFlag(CharacterType.Numerics))
                characterRangeList.Add(_rangeSwitch.Case(CharacterType.Numerics));

            if (characterType.HasFlag(CharacterType.Symbols))
                characterRangeList.Add(_rangeSwitch.Case(CharacterType.Symbols));

            return characterRangeList.ToArray();
        }
    }
}
