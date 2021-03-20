import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { tap } from 'rxjs/operators';

@Component({
  selector: 'app-student-view',
  templateUrl: './student-view.component.html',
  styleUrls: ['./student-view.component.scss']
})
export class StudentViewComponent implements OnInit {
  public studentId: string;
  public moduleId: number;

  constructor(
    private activatedRoute: ActivatedRoute
  ) { }

  ngOnInit(): void {
    this.initializeRouteParams();
  }

  private initializeRouteParams(): void {
    this.activatedRoute.params.pipe(
      tap(params => this.studentId = params.id),
      tap(params => this.moduleId = +params.moduleId)
    )
      .subscribe();
  }
}
