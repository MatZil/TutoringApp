import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { tap } from 'rxjs/operators';
import { IgnoredStudent } from 'src/app/models/users/ignored-student';
import { UsersService } from 'src/app/services/users/users.service';

@Component({
  selector: 'app-user-ignores-sidebar',
  templateUrl: './user-ignores-sidebar.component.html',
  styleUrls: ['./user-ignores-sidebar.component.scss'],
  providers: [
    ConfirmationService,
    MessageService
  ]
})
export class UserIgnoresSidebarComponent implements OnInit {
  public isVisible = false;

  public ignoredStudents: IgnoredStudent[] = [];

  constructor(
    private usersService: UsersService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
  }

  public openSidebar(): void {
    this.initializeIgnoredStudents();
    this.isVisible = true;
  }

  private initializeIgnoredStudents(): void {
    this.usersService.getIgnoredStudents().pipe(
      tap(is => this.ignoredStudents = is)
    )
      .subscribe();
  }

  public confirmStudentUnignore(studentId: string): void {
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: 'Are you sure you want to remove this student from your ignore list?',
      accept: () => this.unignoreStudent(studentId)
    });
  }

  private unignoreStudent(studentId: string): void {
    this.usersService.unignoreStudent(studentId).pipe(
      tap(_ => this.ignoredStudents = this.ignoredStudents.filter(s => s.id !== studentId))
    )
      .subscribe({ error: err => this.messageService.add({ severity: 'error', detail: err.error }) });
  }
}
