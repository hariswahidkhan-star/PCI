"""Loader self-test: proves the ctx contract and the fail-closed behaviour. Not a domain module."""


def run(ctx):
    check, D, af = ctx["check"], ctx["D"], ctx["af"]
    check("loader ctx: Decimal arithmetic is exact", D("0.1") * 3, D("0.3"))
    check("loader ctx: af(6%,12) matches the shared helper",
          af(D("0.06"), 12).quantize(D("0.000001")), D("8.383844"), tol=D("0.0000005"))
    check("loader ctx: Kestrel instalment constant", ctx["KESTREL_INSTALMENT"], D("5009635.23"))
    check("loader ctx: Meridian cost of delay constant", ctx["MERIDIAN_COST_OF_DELAY"], 14280)
