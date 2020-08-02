import { MatPaginatorIntl } from '@angular/material/paginator';

const plRangeLabel = (page: number, pageSize: number, length: number) => {
  if (length == 0 || pageSize == 0) { return `0 z ${length}`; }
  
  length = Math.max(length, 0);

  const startIndex = page * pageSize;

  // If the start index exceeds the list length, do not try and fix the end index to the end.
  const endIndex = startIndex < length ?
      Math.min(startIndex + pageSize, length) :
      startIndex + pageSize;

  return `${startIndex + 1} - ${endIndex} z ${length}`;
}


export function getPlPaginatorIntl() {
  const paginatorIntl = new MatPaginatorIntl();
  
  paginatorIntl.getRangeLabel = plRangeLabel;
  paginatorIntl.nextPageLabel = "Następna";
  paginatorIntl.previousPageLabel = "Poprzednia";

  return paginatorIntl;
}