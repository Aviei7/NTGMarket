import { BACKEND_BASE_URL } from '../config/api.config';

type ProductPlaceholderKind = 'generic' | 'laptop' | 'snack' | 'watch';

const PRODUCT_IMAGE_BASE_PATH = `${BACKEND_BASE_URL}/images/product`;
const PLACEHOLDER_BASE_PATH = '/assets/placeholders';

function normalizeImagePath(imagePath: string): string {
  if (
    imagePath.startsWith('http://') ||
    imagePath.startsWith('https://') ||
    imagePath.startsWith('data:') ||
    imagePath.startsWith('/')
  ) {
    return imagePath;
  }

  return `${PRODUCT_IMAGE_BASE_PATH}/${encodeURIComponent(imagePath)}`;
}

function detectPlaceholderKind(
  categoryName?: string | null,
  productName?: string | null
): ProductPlaceholderKind {
  const source = `${categoryName ?? ''} ${productName ?? ''}`.toLowerCase();

  if (
    source.includes('laptop') ||
    source.includes('notebook') ||
    source.includes('lenovo') ||
    source.includes('pc')
  ) {
    return 'laptop';
  }

  if (
    source.includes('watch') ||
    source.includes('xiaomi') ||
    source.includes('smart')
  ) {
    return 'watch';
  }

  if (
    source.includes('food') ||
    source.includes('leys') ||
    source.includes('chips') ||
    source.includes('snack') ||
    source.includes('crab')
  ) {
    return 'snack';
  }

  return 'generic';
}

export function getProductPlaceholderUrl(
  categoryName?: string | null,
  productName?: string | null
): string {
  const kind = detectPlaceholderKind(categoryName, productName);
  return `${PLACEHOLDER_BASE_PATH}/product-${kind}.svg`;
}

export function buildProductImageUrl(
  imagePath?: string | null,
  categoryName?: string | null,
  productName?: string | null
): string {
  const normalizedPath = imagePath?.trim();

  if (!normalizedPath) {
    return getProductPlaceholderUrl(categoryName, productName);
  }

  return normalizeImagePath(normalizedPath);
}

export function swapToProductPlaceholder(
  event: Event,
  categoryName?: string | null,
  productName?: string | null
): void {
  const target = event.target as HTMLImageElement | null;

  if (!target) {
    return;
  }

  const placeholderUrl = getProductPlaceholderUrl(categoryName, productName);

  if (!target.currentSrc.includes(placeholderUrl)) {
    target.src = placeholderUrl;
  }
}
