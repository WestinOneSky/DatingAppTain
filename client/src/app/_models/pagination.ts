export interface Pagination {
    currentPage: number;
    itemsPerAge: number;
    totalItems: number;
    totalPages: number;
}

export class PaginatedResult<T>{
    results: T;
    pagination: Pagination;
}