export const isInvalidDateRange = (
  active?: string,
  expired?: string,
): boolean => {
  if (!active || !expired) return false;
  return new Date(active).getTime() >= new Date(expired).getTime();
};
