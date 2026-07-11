import { useEffect, useRef, useState } from 'react'

/** Animated numeral — counts from 0 to the target on mount (900ms, eased), renders the plain
 * value immediately when the user prefers reduced motion or the value is not a simple number. */
export default function CountUp({ value }: { value: number | string }) {
  const target = typeof value === 'number' ? value : Number(value)
  const animatable =
    Number.isFinite(target) && Math.abs(target) <= 1_000_000 &&
    !(typeof window !== 'undefined' && window.matchMedia?.('(prefers-reduced-motion: reduce)').matches)
  const [shown, setShown] = useState(animatable ? 0 : target)
  const raf = useRef<number>(0)

  useEffect(() => {
    if (!animatable) {
      setShown(target)
      return
    }
    const t0 = performance.now()
    const dur = 900
    const isInt = Number.isInteger(target)
    const tick = (t: number) => {
      const p = Math.min(1, (t - t0) / dur)
      const eased = 1 - Math.pow(1 - p, 3)
      const v = target * eased
      setShown(isInt ? Math.round(v) : Math.round(v * 10) / 10)
      if (p < 1) raf.current = requestAnimationFrame(tick)
    }
    raf.current = requestAnimationFrame(tick)
    return () => cancelAnimationFrame(raf.current)
  }, [target, animatable])

  if (!Number.isFinite(target)) return <>{value}</>
  return <>{shown}</>
}
