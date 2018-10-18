import { Component, OnInit } from '@angular/core';
import { Hero } from '../hero';
import { HeroService } from '../hero.service';
import { TrackedItem } from '../trackeditem';
import { ClicktrackService } from '../clicktrack.service';
import { map } from 'rxjs/operators';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: [ './dashboard.component.css' ]
})
export class DashboardComponent implements OnInit {
  heroes: Hero[] = [];

  constructor(
    private heroService: HeroService,
    private clickTrackService: ClicktrackService) { }

  ngOnInit() {
    this.getHeroes();
  }

  click(hero: Hero): void {

    var clickInfo: TrackedItem = {
      id: hero.id,
      name: hero.name,
      element: "tophero",
      metadata: "topheroselect",
    }
    this.clickTrackService.track(clickInfo)
      .subscribe();
  }
  getHeroes(): void {
    this.heroService.getHeroes()
      .subscribe(heroes => this.heroes = heroes.slice(1, 5));
  }
}