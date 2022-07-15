export interface Pagination {
    currentPage: number;
    itemsPerPage: number;
    totalItems: number;
    totalPages: number;
}
// <T> is basically <Member[]> in our case 
export class PaginatedResult<T> {
    result: T;
    pagination: Pagination;
}
