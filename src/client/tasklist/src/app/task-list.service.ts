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
      .then(response => {
        if (response.status === 201) {
          return response.json();
        }

        return null;
      })
      .catch(err => {
        switch (err.status) {
          case 409:
              throw Error(`名为${task.name}的任务项目已存在。`);
        }
      });
  }

  deleteTaskById(id: string): Promise<boolean> {
    const url = `${environment.serviceBaseUrl}?id=${id}`;
    return this.http.delete(url)
      .toPromise()
      .then(response => {
        if (response.status === 204) {
          return true;
        } else {
          return false;
        }
      });
  }
}
