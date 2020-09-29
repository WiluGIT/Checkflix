export interface IUserCollectionFilter {
  pageSize: number;
  pageNumber: number;
  favourites: boolean;
  toWatch: boolean;
  watched: boolean;
  userId: string;
}