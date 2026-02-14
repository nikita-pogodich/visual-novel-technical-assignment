using System;
using UnityEngine;

namespace Settings
{
    [Serializable]
    public class ResourceNames : IResourceNames
    {
        [field: SerializeField]
        public string WindowRoots { get; private set; } = "WindowRoots";

        [field: SerializeField]
        public string PieceWhite { get; private set; } = "CheckersPiece_White";

        [field: SerializeField]
        public string PieceBlack { get; private set; } = "CheckersPiece_Black";

        [field: SerializeField]
        public string DefaultBoardPositions { get; private set; } = "DefaultBoardPositions";

        [field: SerializeField]
        public string WhiteWinConditionsBoard { get; private set; } = "WhiteWinConditionsBoard";

        [field: SerializeField]
        public string BlackWinConditionsBoard { get; private set; } = "BlackWinConditionsBoard";
    }
}