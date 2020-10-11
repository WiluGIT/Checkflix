import { IVodViewModel } from "./IVodViewModel";
import { ICategoryViewModel } from "./ICategoryViewModel";

export interface INotificationFromViewModel {
    notificationId: number;
    date: Date;
    content: string;
    toAll: boolean;
    vods: IVodViewModel[];
    categories: ICategoryViewModel[];
}