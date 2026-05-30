export default function GetPageNumbers(totalPages) {
  const total = Math.max(1, Number(totalPages) || 1);

  return function (currentPage, maxVisible = 5) {
    const current = Math.max(1, Number(currentPage) || 1);

    let start = Math.max(1, current - Math.floor(maxVisible / 2));
    let end = Math.min(total, start + maxVisible - 1);

    if (end - start + 1 < maxVisible) {
      start = Math.max(1, end - maxVisible + 1);
    }

    const pages = [];
    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    return pages;
  };
}
