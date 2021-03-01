import { Component, OnInit } from '@angular/core';
import { ConfirmationService, MessageService } from 'primeng/api';
import { StudentCycleEnum } from 'src/app/models/enums/student-cycle-enum';
import { UserUnconfirmed } from 'src/app/models/users/user-unconfirmed';
import { UsersService } from 'src/app/services/users/users.service';
import { mapStudentYearToNumber } from 'src/app/utils/map-student-year-to-number';

@Component({
  selector: 'app-unconfirmed-user-table',
  templateUrl: './unconfirmed-user-table.component.html',
  styleUrls: ['./unconfirmed-user-table.component.scss'],
  providers: [
    ConfirmationService,
    MessageService
  ]
})
export class UnconfirmedUserTableComponent implements OnInit {
  public users: UserUnconfirmed[] = [];
  public columns = [
    { field: 'name', header: 'Name' },
    { field: 'email', header: 'Email' },
    { field: 'faculty', header: 'Faculty' },
    { field: 'studyBranch', header: 'Branch' },
    { field: 'studentCycleDisplay', header: 'Cycle' },
    { field: 'studentYearDisplay', header: 'Year' }
  ];

  public isRejectionDialogVisible = false;
  public rejectionDialogHeader = '';
  private userToRejectId = '';

  constructor(
    private usersService: UsersService,
    private confirmationService: ConfirmationService,
    private messageService: MessageService
  ) { }

  ngOnInit(): void {
    this.initializeUsers();
  }

  //#region Initialization
  private initializeUsers(): void {
    this.usersService.getUnconfirmedUsers().subscribe(users => {
      this.users = users;
      this.users.map(user => {
        user.studentCycleDisplay = StudentCycleEnum[user.studentCycle];
        user.studentYearDisplay = mapStudentYearToNumber(user.studentYear).toString();
      });
    });
  }
  //#endregion

  //#region User confirmation management
  public confirmUser(user: UserUnconfirmed) {
    this.confirmationService.confirm({
      header: 'Confirmation',
      message: `Are you sure you want to confirm user ${user.name}?`,
      accept: () => this.doConfirmUser(user.id)
    });
  }

  private doConfirmUser(id: string) {
    this.usersService.confirmUser(id).subscribe(
      _ => this.handleConfirmationSuccess(id),
      err => this.messageService.add({ severity: 'error', summary: 'Could not confirm user', detail: err.error })
    );
  }

  private handleConfirmationSuccess(id: string): void {
    this.messageService.add({ severity: 'success', summary: 'Success!', detail: 'User was successfully confirmed.' });

    this.users = this.users.filter(u => u.id !== id);
  }
  //#endregion

  //#region User rejection management
  public openRejectionDialog(user: UserUnconfirmed): void {
    this.rejectionDialogHeader = `Why do you want to reject ${user.name}?`;
    this.userToRejectId = user.id;

    this.setRejectionDialogVisibility(true);
  }

  public rejectUser(rejectionReason: string): void {
    this.setRejectionDialogVisibility(false);
    this.usersService.rejectUser(this.userToRejectId, rejectionReason).subscribe(
      _ => this.handleRejectionSuccess(),
      err => this.messageService.add({ severity: 'error', summary: 'Could not reject user', detail: err.error })
    );
  }

  private handleRejectionSuccess(): void {
    this.messageService.add({ severity: 'success', summary: 'Success!', detail: 'User was successfully rejected.' });

    this.users = this.users.filter(u => u.id !== this.userToRejectId);
  }

  public cancelRejection(): void {
    this.setRejectionDialogVisibility(false);
  }

  private setRejectionDialogVisibility(isVisible: boolean): void {
    this.isRejectionDialogVisible = isVisible;
  }
  //#endregion
}
