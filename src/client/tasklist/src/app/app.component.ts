import { Component, OnInit, Input } from '@angular/core';
import { TaskItem } from './task-item';
import { TaskListService } from './task-list.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  public newTaskName: string;

  tasklist: TaskItem[];

  ngOnInit(): void {
    this.getTaskList();
  }

  constructor(private service: TaskListService) {

  }

  private getTaskList(): void {
    this.service.getTaskList()
      .then(response => this.tasklist = response);
  }

  deleteTask(item: TaskItem): void {
    alert(item.id);
  }
}
