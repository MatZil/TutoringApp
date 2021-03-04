import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';

@Component({
  selector: 'app-user-rejection-dialog',
  templateUrl: './user-rejection-dialog.component.html',
  styleUrls: ['./user-rejection-dialog.component.scss']
})
export class UserRejectionDialogComponent implements OnChanges {
  @Input() public isVisible: boolean;
  @Input() public header: string;

  @Output()
  private userRejected = new EventEmitter<string>();

  @Output()
  private rejectionCancelled = new EventEmitter<boolean>();

  public rejectionReason = '';

  constructor() { }

  ngOnChanges(): void {
    this.rejectionReason = '';
  }

  public rejectUser(): void {
    this.userRejected.emit(this.rejectionReason);
  }

  public cancelRejection(): void {
    this.rejectionCancelled.emit(true);
  }
}
