import './passport.css'

/** Loading placeholder in the Passport's own frame, so the module never jumps between a foreign
 *  spinner and the finished card. The shimmer stops entirely under prefers-reduced-motion. */
export default function PassportSkeleton() {
  return (
    <section className="pp-card pp-skeleton" aria-busy="true" aria-label="Loading your PCI World Passport">
      <div className="pp-shimmer" style={{ width: '38%', height: '0.8rem' }} />
      <div className="pp-shimmer" style={{ width: '58%', height: '1.3rem', marginTop: '0.7rem' }} />
      <div className="pp-shimmer" style={{ width: '30%', height: '0.8rem', marginTop: '0.6rem' }} />
      <div className="pp-stats">
        <div className="pp-shimmer" style={{ width: '5.5rem', height: '2.4rem' }} />
        <div className="pp-shimmer" style={{ width: '5.5rem', height: '2.4rem' }} />
      </div>
    </section>
  )
}
