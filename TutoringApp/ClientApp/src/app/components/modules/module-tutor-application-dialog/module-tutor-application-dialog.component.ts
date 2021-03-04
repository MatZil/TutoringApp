import { Component, EventEmitter, Input, OnChanges, Output } from '@angular/core';
import { TutoringApplicationNew } from 'src/app/models/tutoring/tutoring-application-new';

@Component({
  selector: 'app-module-tutor-application-dialog',
  templateUrl: './module-tutor-application-dialog.component.html',
  styleUrls: ['./module-tutor-application-dialog.component.scss']
})
export class ModuleTutorApplicationDialogComponent implements OnChanges {
  @Input() public isVisible: boolean;
  @Input() public header: string;

  @Output()
  private userApplied = new EventEmitter<TutoringApplicationNew>();

  @Output()
  private applicationCancelled = new EventEmitter<boolean>();

  public motivationalLetter = '';

  constructor() { }

  ngOnChanges(): void {
    this.motivationalLetter = '';
  }

  public apply(): void {
    const tutoringApplicationNew: TutoringApplicationNew = {
      motivationalLetter: this.motivationalLetter
    };

    this.userApplied.emit(tutoringApplicationNew);
  }

  public cancelApplication(): void {
    this.applicationCancelled.emit(true);
  }
}
