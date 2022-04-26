import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Field } from '../models/Field';
import { environment } from 'environments/environment';


@Injectable({
  providedIn: 'root'
})
export class FieldService {

  constructor(private httpClient: HttpClient) { }


  getFieldList(): Observable<Field[]> {

    return this.httpClient.get<Field[]>(environment.getApiUrl + '/fields/getall')
  }

  getFieldById(id: number): Observable<Field> {
    return this.httpClient.get<Field>(environment.getApiUrl + '/fields/getbyid?id='+id)
  }

  addField(field: Field): Observable<any> {

    return this.httpClient.post(environment.getApiUrl + '/fields/', field, { responseType: 'text' });
  }

  updateField(field: Field): Observable<any> {
    return this.httpClient.put(environment.getApiUrl + '/fields/', field, { responseType: 'text' });

  }

  deleteField(id: number) {
    return this.httpClient.request('delete', environment.getApiUrl + '/fields/', { body: { id: id } });
  }


}