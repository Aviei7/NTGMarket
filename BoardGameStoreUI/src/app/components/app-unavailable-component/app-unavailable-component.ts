import { Component } from '@angular/core';

type BackgroundIcon = {
  top: number;
  left: number;
  size: number;
  rotate: number;
  opacity: number;
  zoneClass: string;
};

@Component({
  selector: 'app-unavailable',
  standalone: true,
  imports: [],
  templateUrl: './app-unavailable-component.html',
  styleUrl: './app-unavailable-component.css'
})
export class AppUnavailableComponent {
  protected readonly backgroundIcons: BackgroundIcon[] = this.buildBackgroundIcons();

  private randomBetween(min: number, max: number): number {
    return Math.floor(Math.random() * (max - min + 1)) + min;
  }

  private buildBackgroundIcons(): BackgroundIcon[] {
    const icons: BackgroundIcon[] = [];
    const targetCount = 10;
    const maxAttempts = 400;

    for (let attempt = 0; attempt < maxAttempts && icons.length < targetCount; attempt++) {
      const candidate = this.createCandidate();

      if (this.isInMessageArea(candidate) || this.overlapsExisting(candidate, icons)) {
        continue;
      }

      icons.push(candidate);
    }

    return icons;
  }

  private createCandidate(): BackgroundIcon {
    const size = this.randomBetween(72, 152);
    const zoneClass = size >= 124 ? 'background-icon-large' : 'background-icon-small';

    return {
      top: this.randomBetween(6, 92),
      left: this.randomBetween(4, 94),
      size,
      rotate: this.randomBetween(-35, 35),
      opacity: this.randomBetween(24, 40) / 100,
      zoneClass
    };
  }

  private isInMessageArea(icon: BackgroundIcon): boolean {
    const buffer = this.getRadius(icon) + 4;

    return (
      icon.left > 30 - buffer
      && icon.left < 70 + buffer
      && icon.top > 34 - buffer
      && icon.top < 66 + buffer
    );
  }

  private overlapsExisting(candidate: BackgroundIcon, icons: BackgroundIcon[]): boolean {
    const candidateRadius = this.getRadius(candidate);

    return icons.some(icon => {
      const minDistance = candidateRadius + this.getRadius(icon) + 2;
      const dx = candidate.left - icon.left;
      const dy = candidate.top - icon.top;

      return Math.hypot(dx, dy) < minDistance;
    });
  }

  private getRadius(icon: BackgroundIcon): number {
    return icon.size / 24;
  }
}
