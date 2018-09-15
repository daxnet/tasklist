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

  createTask(): void {
    const task = new TaskItem();
    task.name = this.newTaskName;
    this.service.addTask(task)
      .then(
        response => {this.getTaskList();
        this.newTaskName = '';
      })
      .catch(err => alert(err.message));
  }

  deleteTask(item: TaskItem): void {
    this.service.deleteTaskById(item.id)
      .then(response => this.getTaskList())
      .catch(err => alert(err));
  }
}
