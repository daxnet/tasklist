import { Injectable } from '@angular/core';
import { Http } from '@angular/http';
import 'rxjs/add/operator/toPromise';
import { TaskItem } from './task-item';
import { environment } from '../environments/environment';
import { RequestOptionsBuilder } from './request-options-builder';

@Injectable({
  providedIn: 'root'
})
export class TaskListService {

  private headers = new Headers({ 'Content-Type': 'application/json' });

  constructor(private http: Http) {
  }

  getTaskList(): Promise<TaskItem[]> {
    const url = environment.serviceBaseUrl;
    return this.http.get(url)
      .toPromise()
      .then(response => {
        const json = response.json();
        return json as TaskItem[];
      });
  }

  addTask(task: TaskItem): Promise<string> {
    const url = environment.serviceBaseUrl;
    const body = JSON.stringify(task);
    const requestOptionsBuilder = new RequestOptionsBuilder();
    requestOptionsBuilder.withHeader('Content-Type', 'application/json');
    return this.http.post(url, body, requestOptionsBuilder.build())
      .toPromise()
      .then(response => response.toString());
  }
}
