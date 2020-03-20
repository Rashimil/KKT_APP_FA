namespace KKT_APP_FA.Enums
{
    public enum SystemByteOrderEnum // Системный порядок байт
    {
        // Обратный (сетевой) порядок байт в системе (BIG ENDIAN) (от старшего к младшему), реверсируем ответы
        BE = 1,
        BIG_ENDIAN = 1,

        // Прямой (интеловский) порядок байт в системе (LITTLE ENDIAN) (от младшего к старшему), НЕ реверсируем ответы:
        LE = 2,
        LITTLE_ENDIAN = 2
    }
}
