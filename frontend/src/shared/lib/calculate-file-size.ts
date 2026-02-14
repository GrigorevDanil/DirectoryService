export const calculateFileSize = (fileSize: number): string => {
  if (fileSize < 0) return "0б";

  const units = ["б", "кб", "мб", "гб", "тб"];
  let index = 0;
  let size = fileSize;

  while (size >= 1024 && index < units.length - 1) {
    size /= 1024;
    index++;
  }

  const formattedSize = size % 1 === 0 ? size.toString() : size.toFixed(1);

  return `${formattedSize}${units[index]}`;
};
