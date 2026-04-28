#!/bin/bash

# Расширение файлов, которые нужно скопировать (передаётся как аргумент)
EXT="${1:-cs}"

# Папка назначения (куда копировать)
DEST="${2:-./CollectedFiles}"

# Создаём папку назначения, если её нет
mkdir -p "$DEST"

# Ищем ТОЛЬКО в папке Assets
find ./Assets -type f -name "*.$EXT" -exec cp --parents {} "$DEST" \;

# Удаляем из папки назначения всё, что не имеет расширение $EXT
find "$DEST" -type f ! -name "*.$EXT" -delete

# Удаляем пустые папки
find "$DEST" -type d -empty -delete

echo "Готово! Все .$EXT файлы из папки Assets скопированы в $DEST, остальное удалено."
