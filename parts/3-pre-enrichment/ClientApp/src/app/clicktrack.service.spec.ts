import { TestBed } from '@angular/core/testing';

import { ClicktrackService } from './clicktrack.service';

describe('ClicktrackService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ClicktrackService = TestBed.get(ClicktrackService);
    expect(service).toBeTruthy();
  });
});
