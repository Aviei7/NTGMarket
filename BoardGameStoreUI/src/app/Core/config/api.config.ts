export const BACKEND_BASE_URL = '';

export function buildApiUrl(path: string): string {
  const normalizedPath = path.startsWith('/') ? path : `/${path}`;
  return `/api${normalizedPath}`;
}
