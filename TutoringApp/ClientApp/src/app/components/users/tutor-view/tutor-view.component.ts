import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-tutor-view',
  templateUrl: './tutor-view.component.html',
  styleUrls: ['./tutor-view.component.scss']
})
export class TutorViewComponent implements OnInit {
  public tutorId: number;

  constructor(
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.initializeTutorId();
  }

  private initializeTutorId(): void {
    this.activatedRoute.params.pipe(
      tap(params => this.tutorId = params.id)
    )
      .subscribe();
  }
}
