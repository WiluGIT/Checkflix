import { IVodViewModel } from "./IVodViewModel";
import { ICategoryViewModel } from "./ICategoryViewModel";

export interface IUserPreferencesViewModel {
    vods: IVodViewModel[];
    categories: ICategoryViewModel[];
}