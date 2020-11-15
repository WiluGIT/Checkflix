import { IVodViewModel } from "./IVodViewModel";
import { ICategoryViewModel } from "./ICategoryViewModel";

export interface IProductionViewModel {
  productionId?: number;
  title: string;
  subtitle: string;
  poster: string;
  synopsis: string;
  type: number;
  releaseDate: Date;
  imbdId: string;
  imbdRating: number;
  vods: IVodViewModel[];
  categories: ICategoryViewModel[];
}
