import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IVodCountViewModel } from 'src/app/ClientViewModels/IVodCountViewModel';
import { environment } from 'src/environments/environment';
import { map } from 'rxjs/operators';
import { IVodViewModel } from 'src/app/ClientViewModels/IVodViewModel';

@Injectable({
  providedIn: 'root'
})
export class VodService {
  private getVodCountPath = environment.apiUrl + '/api/vods/GetVodCount';
  private getVodsPath = environment.apiUrl + '/api/vods/GetVods';

  constructor(private http: HttpClient) { }

  getVodCount(): Observable<IVodCountViewModel> {
    return this.http.get(this.getVodCountPath).pipe(map((countVm: IVodCountViewModel) => countVm));
  }
  
  getVods(): Observable<IVodViewModel[]> {
    return this.http.get(this.getVodsPath).pipe(map((vods: IVodViewModel[]) => vods));
  }
}
