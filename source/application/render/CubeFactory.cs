namespace Render.Examples
{
    public static class CubeFactoryData
    {
        public static readonly float[] CubeVertices =
        {
            // CARA FRENTE (Z = 1.0)
            // Posición         // Normal          // Textura
            -1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f, 0.0f, // V0
            1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f, 0.0f, // V1
            1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  1.0f,  1.0f, 1.0f, // V2
            -1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  1.0f,  0.0f, 1.0f, // V3

            // CARA ATRÁS (Z = -1.0)
            1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f,  0.0f, 0.0f, // V4 (Índice 4)
            -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f,  1.0f, 0.0f, // V5
            -1.0f,  1.0f, -1.0f,  0.0f,  0.0f, -1.0f,  1.0f, 1.0f, // V6
            1.0f,  1.0f, -1.0f,  0.0f,  0.0f, -1.0f,  0.0f, 1.0f, // V7

            // CARA ARRIBA (Y = 1.0)
            -1.0f,  1.0f,  1.0f,  0.0f,  1.0f,  0.0f,  0.0f, 0.0f, // V8 (Índice 8)
            1.0f,  1.0f,  1.0f,  0.0f,  1.0f,  0.0f,  1.0f, 0.0f, // V9
            1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  0.0f,  1.0f, 1.0f, // V10
            -1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  0.0f,  0.0f, 1.0f, // V11

            // CARA ABAJO (Y = -1.0)
            -1.0f, -1.0f, -1.0f,  0.0f, -1.0f,  0.0f,  0.0f, 0.0f, // V12 (Índice 12)
            1.0f, -1.0f, -1.0f,  0.0f, -1.0f,  0.0f,  1.0f, 0.0f, // V13
            1.0f, -1.0f,  1.0f,  0.0f, -1.0f,  0.0f,  1.0f, 1.0f, // V14
            -1.0f, -1.0f,  1.0f,  0.0f, -1.0f,  0.0f,  0.0f, 1.0f, // V15

            // CARA DERECHA (X = 1.0)
            1.0f, -1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  0.0f, 0.0f, // V16 (Índice 16)
            1.0f, -1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  1.0f, 0.0f, // V17
            1.0f,  1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  1.0f, 1.0f, // V18
            1.0f,  1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  0.0f, 1.0f, // V19

            // CARA IZQUIERDA (X = -1.0)
            -1.0f, -1.0f, -1.0f, -1.0f,  0.0f,  0.0f,  0.0f, 0.0f, // V20 (Índice 20)
            -1.0f, -1.0f,  1.0f, -1.0f,  0.0f,  0.0f,  1.0f, 0.0f, // V21
            -1.0f,  1.0f,  1.0f, -1.0f,  0.0f,  0.0f,  1.0f, 1.0f, // V22
            -1.0f,  1.0f, -1.0f, -1.0f,  0.0f,  0.0f,  0.0f, 1.0f  // V23
        };

        public static readonly uint[] CubeIndices =
        {
            // Cara Frente (V0, V1, V2, V3)
            0,  1,  2, // Triángulo 1
            2,  3,  0, // Triángulo 2

            // Cara Atrás (V4, V5, V6, V7)
            4,  5,  6, // Triángulo 3
            6,  7,  4, // Triángulo 4

            // Cara Arriba (V8, V9, V10, V11)
            8,  9, 10, // Triángulo 5
            10, 11,  8, // Triángulo 6

            // Cara Abajo (V12, V13, V14, V15)
            12, 13, 14, // Triángulo 7
            14, 15, 12, // Triángulo 8

            // Cara Derecha (V16, V17, V18, V19)
            16, 17, 18, // Triángulo 9
            18, 19, 16, // Triángulo 10

            // Cara Izquierda (V20, V21, V22, V23)
            20, 21, 22, // Triángulo 11
            22, 23, 20  // Triángulo 12
        };
    }
}